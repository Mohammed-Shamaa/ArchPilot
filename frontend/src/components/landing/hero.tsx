"use client";

import { useTranslations, useLocale } from "next-intl";
import Link from "next/link";
import { Button } from "@/components/ui/button";
import { ArrowRight, Play } from "lucide-react";

export function HeroSection() {
  const t = useTranslations("hero");
  const locale = useLocale();

  return (
    <section className="relative overflow-hidden py-20 md:py-32">
      <div className="absolute inset-0 bg-gradient-to-br from-primary/5 via-transparent to-primary/5" />
      <div className="container mx-auto px-4 relative">
        <div className="max-w-3xl mx-auto text-center space-y-8">
          <div className="inline-flex items-center rounded-full border bg-muted px-3 py-1 text-sm">
            <span className="mr-2 h-2 w-2 rounded-full bg-green-500 animate-pulse" />
            {t("badge")}
          </div>

          <h1 className="text-4xl md:text-6xl font-bold tracking-tight">
            {t("title")}{" "}
            <span className="text-primary bg-gradient-to-r from-primary/70 to-primary bg-clip-text">
              {t("titleHighlight")}
            </span>
          </h1>

          <p className="text-lg md:text-xl text-muted-foreground max-w-2xl mx-auto">
            {t("subtitle")}
          </p>

          <div className="flex flex-col sm:flex-row items-center justify-center gap-4">
            <Link href={`/${locale}/register`}>
              <Button size="lg" className="gap-2">
                {t("cta")}
                <ArrowRight className="h-4 w-4" />
              </Button>
            </Link>
            <Button variant="outline" size="lg" className="gap-2">
              <Play className="h-4 w-4" />
              {t("ctaSecondary")}
            </Button>
          </div>

          <div className="grid grid-cols-3 gap-8 pt-12 max-w-md mx-auto">
            <div className="text-center">
              <div className="text-2xl font-bold">{t("stat1Value")}</div>
              <div className="text-sm text-muted-foreground">
                {t("stat1Label")}
              </div>
            </div>
            <div className="text-center">
              <div className="text-2xl font-bold">{t("stat2Value")}</div>
              <div className="text-sm text-muted-foreground">
                {t("stat2Label")}
              </div>
            </div>
            <div className="text-center">
              <div className="text-2xl font-bold">{t("stat3Value")}</div>
              <div className="text-sm text-muted-foreground">
                {t("stat3Label")}
              </div>
            </div>
          </div>
        </div>
      </div>
    </section>
  );
}
