"use client";

import { useTranslations } from "next-intl";
import { Card, CardHeader, CardTitle, CardDescription } from "@/components/ui/card";
import { Bot, FileText, Download, Brain, Users, Shield } from "lucide-react";

const icons = [Bot, FileText, Download, Brain, Users, Shield];
const keys = ["multiAgent", "documents", "export", "context", "collaboration", "security"] as const;

export function FeaturesSection() {
  const t = useTranslations("features");

  return (
    <section id="features" className="py-20 md:py-32 bg-muted/30">
      <div className="container mx-auto px-4">
        <div className="text-center mb-16 space-y-4">
          <h2 className="text-3xl md:text-4xl font-bold">{t("sectionTitle")}</h2>
          <p className="text-muted-foreground max-w-2xl mx-auto text-lg">
            {t("sectionSubtitle")}
          </p>
        </div>

        <div className="grid grid-cols-1 md:grid-cols-2 lg:grid-cols-3 gap-6">
          {keys.map((key, i) => {
            const Icon = icons[i];
            return (
              <Card key={key} className="relative overflow-hidden hover:shadow-lg transition-all duration-300 hover:-translate-y-1 border-0 bg-background/80 backdrop-blur">
                <CardHeader>
                  <div className="mb-2 h-12 w-12 rounded-xl bg-primary/10 flex items-center justify-center">
                    <Icon className="h-6 w-6 text-primary" />
                  </div>
                  <CardTitle className="text-xl">{t(`${key}.title`)}</CardTitle>
                  <CardDescription className="text-base leading-relaxed">
                    {t(`${key}.description`)}
                  </CardDescription>
                </CardHeader>
              </Card>
            );
          })}
        </div>
      </div>
    </section>
  );
}
