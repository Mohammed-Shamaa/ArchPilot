"use client";

import { useState, useRef, useEffect } from "react";
import { useTranslations, useLocale } from "next-intl";
import { useAuthStore } from "@/stores/auth-store";
import { apiFetch } from "@/lib/api";
import { Button } from "@/components/ui/button";
import { Input } from "@/components/ui/input";
import { Card } from "@/components/ui/card";
import { Send, Bot, User, Loader2, FileText } from "lucide-react";

interface Message {
  id: string;
  content: string;
  senderType: "user" | "ai";
  agentType?: string;
  createdAt: string;
}

export default function ProjectPage({ params }: { params: Promise<{ locale: string; id: string }> }) {
  const [locale, setLocale] = useState("");
  const [projectId, setProjectId] = useState("");
  const t = useTranslations("nav");
  const { token, isAuthenticated } = useAuthStore();
  const [messages, setMessages] = useState<Message[]>([]);
  const [input, setInput] = useState("");
  const [sending, setSending] = useState(false);
  const [documents, setDocuments] = useState<{ id: string; title: string; type: string }[]>([]);
  const messagesEndRef = useRef<HTMLDivElement>(null);

  useEffect(() => {
    params.then(({ locale: loc, id }) => {
      setLocale(loc);
      setProjectId(id);
    });
  }, [params]);

  useEffect(() => {
    if (projectId && token) {
      loadMessages();
      loadDocuments();
    }
  }, [projectId, token]);

  useEffect(() => {
    messagesEndRef.current?.scrollIntoView({ behavior: "smooth" });
  }, [messages]);

  const loadMessages = async () => {
    try {
      const data = await apiFetch<{ items: Message[] }>(`/ai/projects/${projectId}/messages`, {
        token: token || undefined,
      });
      setMessages(data.items || []);
    } catch {}
  };

  const loadDocuments = async () => {
    try {
      const data = await apiFetch<{ items: { id: string; title: string; type: string }[] }>(
        `/documents/projects/${projectId}`,
        { token: token || undefined }
      );
      setDocuments(data.items || []);
    } catch {}
  };

  const sendMessage = async (e: React.FormEvent) => {
    e.preventDefault();
    if (!input.trim() || sending) return;

    const userMsg: Message = {
      id: Date.now().toString(),
      content: input,
      senderType: "user",
      createdAt: new Date().toISOString(),
    };
    setMessages((prev) => [...prev, userMsg]);
    setInput("");
    setSending(true);

    try {
      const data = await apiFetch<{ response: string; agentType: string }>(
        `/ai/projects/${projectId}/chat`,
        {
          method: "POST",
          token: token || undefined,
          body: { message: input },
        }
      );

      const aiMsg: Message = {
        id: (Date.now() + 1).toString(),
        content: data.response,
        senderType: "ai",
        agentType: data.agentType,
        createdAt: new Date().toISOString(),
      };
      setMessages((prev) => [...prev, aiMsg]);
    } catch {
      // handle error
    } finally {
      setSending(false);
    }
  };

  return (
    <div className="flex h-[calc(100vh-56px)]">
      {/* Chat Panel */}
      <div className="flex-1 flex flex-col">
        <div className="flex-1 overflow-y-auto p-4 space-y-4">
          {messages.length === 0 && (
            <div className="flex items-center justify-center h-full text-muted-foreground">
              <div className="text-center space-y-2">
                <Bot className="h-12 w-12 mx-auto" />
                <p>Start a conversation about your project</p>
              </div>
            </div>
          )}
          {messages.map((msg) => (
            <div
              key={msg.id}
              className={`flex gap-3 ${msg.senderType === "user" ? "justify-end" : "justify-start"}`}
            >
              <div
                className={`max-w-[70%] rounded-lg px-4 py-3 ${
                  msg.senderType === "user"
                    ? "bg-primary text-primary-foreground"
                    : "bg-muted"
                }`}
              >
                <div className="flex items-center gap-2 mb-1">
                  {msg.senderType === "user" ? (
                    <User className="h-3 w-3" />
                  ) : (
                    <Bot className="h-3 w-3" />
                  )}
                  <span className="text-xs opacity-70">
                    {msg.senderType === "ai" && msg.agentType
                      ? msg.agentType
                      : "You"}
                  </span>
                </div>
                <p className="text-sm whitespace-pre-wrap">{msg.content}</p>
              </div>
            </div>
          ))}
          {sending && (
            <div className="flex gap-3">
              <div className="bg-muted rounded-lg px-4 py-3">
                <Loader2 className="h-4 w-4 animate-spin" />
              </div>
            </div>
          )}
          <div ref={messagesEndRef} />
        </div>

        <div className="border-t p-4">
          <form onSubmit={sendMessage} className="flex gap-2">
            <Input
              value={input}
              onChange={(e) => setInput(e.target.value)}
              placeholder="Describe your software idea or ask a question..."
              disabled={sending}
              className="flex-1"
            />
            <Button type="submit" disabled={sending || !input.trim()}>
              <Send className="h-4 w-4" />
            </Button>
          </form>
        </div>
      </div>

      {/* Documents Panel */}
      <div className="w-80 border-l bg-muted/30 overflow-y-auto hidden lg:block">
        <div className="p-4">
          <h3 className="font-semibold mb-4 flex items-center gap-2">
            <FileText className="h-4 w-4" />
            Documents
          </h3>
          {documents.length === 0 ? (
            <p className="text-sm text-muted-foreground">
              Documents will appear here as the AI generates them.
            </p>
          ) : (
            <div className="space-y-2">
              {documents.map((doc) => (
                <Card key={doc.id} className="p-3 hover:shadow cursor-pointer transition-shadow">
                  <p className="text-sm font-medium">{doc.title}</p>
                  <p className="text-xs text-muted-foreground">{doc.type}</p>
                </Card>
              ))}
            </div>
          )}
        </div>
      </div>
    </div>
  );
}
