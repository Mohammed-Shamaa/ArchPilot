"use client";

import { useState, useRef, useEffect } from "react";
import { useTranslations } from "next-intl";
import { useAuthStore } from "@/stores/auth-store";
import { apiFetch, getApiBase } from "@/lib/api";
import { Button } from "@/components/ui/button";
import { Input } from "@/components/ui/input";
import { Card } from "@/components/ui/card";
import { Send, Bot, User, Loader2, FileText, ArrowLeft, Download } from "lucide-react";
import Link from "next/link";
import { useParams } from "next/navigation";

interface Message {
  id: string;
  content: string;
  senderType: "user" | "ai";
  agentType?: string;
  createdAt: string;
}

interface Document {
  id: string;
  title: string;
  documentType: string;
  createdAt: string;
}

export default function ProjectPage() {
  const { locale, id: projectId } = useParams<{ locale: string; id: string }>();
  const { token } = useAuthStore();
  const [messages, setMessages] = useState<Message[]>([]);
  const [input, setInput] = useState("");
  const [sending, setSending] = useState(false);
  const [documents, setDocuments] = useState<Document[]>([]);
  const [conversationId, setConversationId] = useState<string | null>(null);
  const messagesEndRef = useRef<HTMLDivElement>(null);

  useEffect(() => {
    if (projectId && token) {
      loadDocuments();
    }
  }, [projectId, token]);

  useEffect(() => {
    messagesEndRef.current?.scrollIntoView({ behavior: "smooth" });
  }, [messages]);

  const loadDocuments = async () => {
    try {
      const data = await apiFetch<Document[]>(
        `/documents/project/${projectId}`,
        { token: token || undefined }
      );
      setDocuments(data || []);
    } catch {
      // Documents API may not have data yet
    }
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
    const currentInput = input;
    setInput("");
    setSending(true);

    try {
      const data = await apiFetch<{
        answer: string;
        conversationId: string;
        documentCreated: boolean;
        documentType?: string;
      }>("/ai/chat", {
        method: "POST",
        token: token || undefined,
        body: {
          projectId,
          message: currentInput,
          conversationId: conversationId || undefined,
        },
      });

      if (data.conversationId) {
        setConversationId(data.conversationId);
      }

      const aiMsg: Message = {
        id: (Date.now() + 1).toString(),
        content: data.answer,
        senderType: "ai",
        agentType: data.documentType || undefined,
        createdAt: new Date().toISOString(),
      };
      setMessages((prev) => [...prev, aiMsg]);

      if (data.documentCreated) {
        loadDocuments();
      }
    } catch (err: any) {
      const errorMsg: Message = {
        id: (Date.now() + 1).toString(),
        content: err.message || "Sorry, an error occurred. Please try again.",
        senderType: "ai",
        createdAt: new Date().toISOString(),
      };
      setMessages((prev) => [...prev, errorMsg]);
    } finally {
      setSending(false);
    }
  };

  const exportDocument = async (docId: string, format: string) => {
    try {
      const res = await fetch(
        `${getApiBase()}/documents/${docId}/export?format=${format}`,
        {
          headers: { Authorization: `Bearer ${token}` },
        }
      );
      if (res.ok) {
        const blob = await res.blob();
        const url = URL.createObjectURL(blob);
        const a = window.document.createElement("a");
        a.href = url;
        a.download = `archpilot-document.${format}`;
        a.click();
        URL.revokeObjectURL(url);
      }
    } catch {
      // handle error
    }
  };

  return (
    <div className="flex fixed inset-0 top-14 z-10">
      {/* Chat Panel */}
      <div className="flex-1 flex flex-col">
        <div className="border-b px-4 py-3 flex items-center gap-3">
          <Link href={`/${locale}/dashboard`}>
            <Button variant="ghost" size="icon" className="h-8 w-8">
              <ArrowLeft className="h-4 w-4" />
            </Button>
          </Link>
          <h2 className="font-semibold text-sm">AI Workspace</h2>
        </div>

        <div className="flex-1 overflow-y-auto p-4 space-y-4">
          {messages.length === 0 && (
            <div className="flex items-center justify-center h-full text-muted-foreground">
              <div className="text-center space-y-4">
                <Bot className="h-16 w-16 mx-auto opacity-50" />
                <div className="space-y-2">
                  <p className="text-lg font-medium">Start Building Your Blueprint</p>
                  <p className="text-sm max-w-md">
                    Describe your software idea or ask questions about architecture,
                    database design, APIs, testing, and more.
                  </p>
                </div>
                <div className="flex flex-wrap justify-center gap-2 mt-4">
                  {[
                    "Design a REST API for an e-commerce app",
                    "Create an ERD for a social media platform",
                    "Write an SRS for a ride-sharing app",
                  ].map((suggestion) => (
                    <button
                      key={suggestion}
                      onClick={() => setInput(suggestion)}
                      className="text-xs px-3 py-1.5 rounded-full border bg-muted/50 hover:bg-muted transition-colors"
                    >
                      {suggestion}
                    </button>
                  ))}
                </div>
              </div>
            </div>
          )}

          {messages.map((msg) => (
            <div
              key={msg.id}
              className={`flex gap-3 ${msg.senderType === "user" ? "justify-end" : "justify-start"}`}
            >
              <div
                className={`max-w-[80%] rounded-xl px-4 py-3 ${
                  msg.senderType === "user"
                    ? "bg-primary text-primary-foreground"
                    : "bg-muted"
                }`}
              >
                <div className="flex items-center gap-2 mb-1.5">
                  {msg.senderType === "user" ? (
                    <User className="h-3 w-3" />
                  ) : (
                    <Bot className="h-3 w-3" />
                  )}
                  <span className="text-xs opacity-70">
                    {msg.senderType === "ai"
                      ? msg.agentType || "ArchPilot AI"
                      : "You"}
                  </span>
                </div>
                <p className="text-sm whitespace-pre-wrap leading-relaxed">{msg.content}</p>
              </div>
            </div>
          ))}
          {sending && (
            <div className="flex gap-3 justify-start">
              <div className="bg-muted rounded-xl px-4 py-3 flex items-center gap-2">
                <Loader2 className="h-4 w-4 animate-spin" />
                <span className="text-sm text-muted-foreground">Thinking...</span>
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
            <Button type="submit" disabled={sending || !input.trim()} size="icon">
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
            Generated Documents
          </h3>
          {documents.length === 0 ? (
            <div className="text-center py-8 space-y-2">
              <FileText className="h-8 w-8 mx-auto text-muted-foreground/50" />
              <p className="text-sm text-muted-foreground">
                Documents will appear here as the AI generates them.
              </p>
            </div>
          ) : (
            <div className="space-y-2">
              {documents.map((doc) => (
                <Card key={doc.id} className="p-3 hover:shadow transition-shadow">
                  <div className="flex items-start justify-between">
                    <div className="flex-1 min-w-0">
                      <p className="text-sm font-medium truncate">{doc.title}</p>
                      <p className="text-xs text-muted-foreground mt-0.5">{doc.documentType}</p>
                    </div>
                    <div className="flex gap-1 ml-2">
                      <Button
                        variant="ghost"
                        size="icon"
                        className="h-7 w-7"
                        onClick={() => exportDocument(doc.id, "pdf")}
                        title="Export PDF"
                      >
                        <Download className="h-3 w-3" />
                      </Button>
                    </div>
                  </div>
                </Card>
              ))}
            </div>
          )}
        </div>
      </div>
    </div>
  );
}
