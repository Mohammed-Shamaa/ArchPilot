"use client";

import { useState, useEffect } from "react";
import { useTranslations, useLocale } from "next-intl";
import { useRouter } from "next/navigation";
import { Button } from "@/components/ui/button";
import { Card, CardHeader, CardTitle, CardDescription, CardContent } from "@/components/ui/card";
import { Input } from "@/components/ui/input";
import { useAuthStore } from "@/stores/auth-store";
import { apiFetch } from "@/lib/api";
import { Plus, FolderOpen, MessageSquare, FileText, Loader2, ArrowRight } from "lucide-react";

interface Project {
  id: string;
  projectName: string;
  description?: string;
  status: string;
  createdAt: string;
  lastAccessedAt: string;
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
  const [error, setError] = useState("");

  useEffect(() => {
    if (!isAuthenticated) {
      router.push(`/${locale}/login`);
      return;
    }
    loadProjects();
  }, [isAuthenticated]);

  const loadProjects = async () => {
    try {
      const data = await apiFetch<Project[]>("/projects", { token: token || undefined });
      setProjects(data || []);
    } catch {
      // Projects may not exist yet
    } finally {
      setLoading(false);
    }
  };

  const createProject = async (e: React.FormEvent) => {
    e.preventDefault();
    setCreating(true);
    setError("");
    try {
      const project = await apiFetch<Project>("/projects", {
        method: "POST",
        token: token || undefined,
        body: { ProjectName: newName, Description: newIdea },
      });
      setProjects((prev) => [project, ...prev]);
      setShowNewProject(false);
      setNewName("");
      setNewIdea("");
      router.push(`/${locale}/project/${project.id}`);
    } catch (err: any) {
      setError(err.message || "Failed to create project");
    } finally {
      setCreating(false);
    }
  };

  return (
    <div className="container mx-auto px-4 py-8 max-w-6xl">
      <div className="flex flex-col sm:flex-row items-start sm:items-center justify-between mb-8 gap-4">
        <div>
          <h1 className="text-3xl font-bold">{t("projects")}</h1>
          {user && (
            <p className="text-muted-foreground mt-1">
              {t("welcome")}, <span className="font-medium text-foreground">{user.username}</span>
            </p>
          )}
        </div>
        <Button onClick={() => setShowNewProject(true)} className="gap-2">
          <Plus className="h-4 w-4" />
          {t("newProject")}
        </Button>
      </div>

      {showNewProject && (
        <Card className="mb-8 border-primary/20">
          <CardHeader>
            <CardTitle>{t("newProject")}</CardTitle>
            <CardDescription>Describe your software idea and let AI generate the blueprint.</CardDescription>
          </CardHeader>
          <CardContent>
            <form onSubmit={createProject} className="space-y-4">
              {error && (
                <div className="rounded-md bg-destructive/10 border border-destructive/20 p-3 text-sm text-destructive">
                  {error}
                </div>
              )}
              <div className="space-y-2">
                <label className="text-sm font-medium">{tp("name")}</label>
                <Input
                  value={newName}
                  onChange={(e) => setNewName(e.target.value)}
                  placeholder="My Awesome App"
                  required
                />
              </div>
              <div className="space-y-2">
                <label className="text-sm font-medium">{tp("idea")}</label>
                <textarea
                  className="flex min-h-[140px] w-full rounded-md border border-input bg-transparent px-3 py-2 text-sm shadow-sm placeholder:text-muted-foreground focus-visible:outline-none focus-visible:ring-1 focus-visible:ring-ring resize-none"
                  value={newIdea}
                  onChange={(e) => setNewIdea(e.target.value)}
                  placeholder={tp("ideaPlaceholder")}
                />
              </div>
              <div className="flex gap-2">
                <Button type="submit" disabled={creating} className="gap-2">
                  {creating && <Loader2 className="h-4 w-4 animate-spin" />}
                  {creating ? "Creating..." : tc("submit")}
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
          <div className="flex flex-col items-center gap-3">
            <Loader2 className="h-8 w-8 animate-spin text-muted-foreground" />
            <p className="text-sm text-muted-foreground">Loading projects...</p>
          </div>
        </div>
      ) : projects.length === 0 ? (
        <div className="text-center py-20 space-y-4">
          <FolderOpen className="h-16 w-16 mx-auto text-muted-foreground/40" />
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
              className="hover:shadow-lg transition-all duration-200 cursor-pointer group"
              onClick={() => router.push(`/${locale}/project/${project.id}`)}
            >
              <CardHeader>
                <CardTitle className="flex items-center gap-2 group-hover:text-primary transition-colors">
                  <FolderOpen className="h-5 w-5" />
                  {project.projectName}
                  <ArrowRight className="h-4 w-4 ml-auto opacity-0 group-hover:opacity-100 transition-opacity" />
                </CardTitle>
                <CardDescription className="line-clamp-2">
                  {project.description || "No description"}
                </CardDescription>
              </CardHeader>
              <CardContent>
                <div className="flex items-center justify-between text-sm text-muted-foreground">
                  <div className="flex items-center gap-3">
                    <span className="flex items-center gap-1">
                      <FileText className="h-3 w-3" />
                      0 docs
                    </span>
                    <span className="flex items-center gap-1">
                      <MessageSquare className="h-3 w-3" />
                      0 chats
                    </span>
                  </div>
                  <span className={`text-xs px-2 py-0.5 rounded-full ${
                    project.status === "active" ? "bg-green-100 text-green-700 dark:bg-green-900/30 dark:text-green-400" : "bg-muted"
                  }`}>
                    {project.status || "new"}
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
