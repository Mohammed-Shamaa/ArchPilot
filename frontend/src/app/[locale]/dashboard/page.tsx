"use client";

import { useState, useEffect } from "react";
import { useTranslations, useLocale } from "next-intl";
import { useRouter } from "next/navigation";
import { Button } from "@/components/ui/button";
import { Card, CardHeader, CardTitle, CardDescription, CardContent } from "@/components/ui/card";
import { Input } from "@/components/ui/input";
import { useAuthStore } from "@/stores/auth-store";
import { apiFetch } from "@/lib/api";
import { Plus, FolderOpen, MessageSquare, FileText, Loader2 } from "lucide-react";

interface Project {
  id: string;
  name: string;
  description: string;
  status: string;
  createdAt: string;
}

export default function DashboardPage() {
  const t = useTranslations("dashboard");
  const tc = useTranslations("common");
  const tp = useTranslations("project");
  const locale = useLocale();
  const router = useRouter();
  const { user, token, isAuthenticated } = useAuthStore();
  const [projects, setProjects] = useState<Project[]>([]);
  const [loading, setLoading] = useState(true);
  const [showNewProject, setShowNewProject] = useState(false);
  const [newName, setNewName] = useState("");
  const [newIdea, setNewIdea] = useState("");
  const [creating, setCreating] = useState(false);

  useEffect(() => {
    if (!isAuthenticated) {
      router.push(`/${locale}/login`);
      return;
    }
    loadProjects();
  }, [isAuthenticated]);

  const loadProjects = async () => {
    try {
      const data = await apiFetch<{ items: Project[] }>("/projects", { token: token || undefined });
      setProjects(data.items || []);
    } catch {
      // Projects API may not be fully ready
    } finally {
      setLoading(false);
    }
  };

  const createProject = async (e: React.FormEvent) => {
    e.preventDefault();
    setCreating(true);
    try {
      const project = await apiFetch<Project>("/projects", {
        method: "POST",
        token: token || undefined,
        body: { name: newName, idea: newIdea },
      });
      setProjects((prev) => [project, ...prev]);
      setShowNewProject(false);
      setNewName("");
      setNewIdea("");
    } catch {
      // handle error
    } finally {
      setCreating(false);
    }
  };

  return (
    <div className="container mx-auto px-4 py-8">
      <div className="flex items-center justify-between mb-8">
        <div>
          <h1 className="text-3xl font-bold">{t("projects")}</h1>
          {user && (
            <p className="text-muted-foreground mt-1">
              {t("welcome")}, {user.fullName}
            </p>
          )}
        </div>
        <Button onClick={() => setShowNewProject(true)} className="gap-2">
          <Plus className="h-4 w-4" />
          {t("newProject")}
        </Button>
      </div>

      {showNewProject && (
        <Card className="mb-8">
          <CardHeader>
            <CardTitle>{t("newProject")}</CardTitle>
          </CardHeader>
          <CardContent>
            <form onSubmit={createProject} className="space-y-4">
              <div className="space-y-2">
                <label className="text-sm font-medium">{tp("name")}</label>
                <Input
                  value={newName}
                  onChange={(e) => setNewName(e.target.value)}
                  required
                />
              </div>
              <div className="space-y-2">
                <label className="text-sm font-medium">{tp("idea")}</label>
                <textarea
                  className="flex min-h-[120px] w-full rounded-md border border-input bg-transparent px-3 py-2 text-sm shadow-sm placeholder:text-muted-foreground focus-visible:outline-none focus-visible:ring-1 focus-visible:ring-ring"
                  value={newIdea}
                  onChange={(e) => setNewIdea(e.target.value)}
                  placeholder={tp("ideaPlaceholder")}
                  required
                />
              </div>
              <div className="flex gap-2">
                <Button type="submit" disabled={creating}>
                  {creating && <Loader2 className="mr-2 h-4 w-4 animate-spin" />}
                  {tc("submit")}
                </Button>
                <Button type="button" variant="outline" onClick={() => setShowNewProject(false)}>
                  {tc("cancel")}
                </Button>
              </div>
            </form>
          </CardContent>
        </Card>
      )}

      {loading ? (
        <div className="flex items-center justify-center py-20">
          <Loader2 className="h-8 w-8 animate-spin text-muted-foreground" />
        </div>
      ) : projects.length === 0 ? (
        <div className="text-center py-20 space-y-4">
          <FolderOpen className="h-16 w-16 mx-auto text-muted-foreground/50" />
          <h3 className="text-xl font-semibold">{t("noProjects")}</h3>
          <p className="text-muted-foreground max-w-md mx-auto">
            {t("noProjectsDesc")}
          </p>
          <Button onClick={() => setShowNewProject(true)} className="gap-2">
            <Plus className="h-4 w-4" />
            {t("createFirst")}
          </Button>
        </div>
      ) : (
        <div className="grid grid-cols-1 md:grid-cols-2 lg:grid-cols-3 gap-6">
          {projects.map((project) => (
            <Card
              key={project.id}
              className="hover:shadow-lg transition-shadow cursor-pointer"
              onClick={() => router.push(`/${locale}/project/${project.id}`)}
            >
              <CardHeader>
                <CardTitle className="flex items-center gap-2">
                  <FolderOpen className="h-5 w-5" />
                  {project.name}
                </CardTitle>
                <CardDescription className="line-clamp-2">
                  {project.description}
                </CardDescription>
              </CardHeader>
              <CardContent>
                <div className="flex items-center justify-between text-sm text-muted-foreground">
                  <span className="flex items-center gap-1">
                    <FileText className="h-3 w-3" />
                    {tp("documents")}
                  </span>
                  <span className="flex items-center gap-1">
                    <MessageSquare className="h-3 w-3" />
                    {tp("conversations")}
                  </span>
                </div>
              </CardContent>
            </Card>
          ))}
        </div>
      )}
    </div>
  );
}
