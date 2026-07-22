"use client";

import { useTranslations } from "next-intl";
import { Lightbulb, Cpu, Eye, Rocket } from "lucide-react";

const steps = [
  { key: "step1", icon: Lightbulb },
  { key: "step2", icon: Cpu },
  { key: "step3", icon: Eye },
  { key: "step4", icon: Rocket },
];

export function WorkflowSection() {
  const t = useTranslations("workflow");

  return (
    <section className="py-20 md:py-32">
      <div className="container mx-auto px-4">
        <div className="text-center mb-16 space-y-4">
          <h2 className="text-3xl md:text-4xl font-bold">{t("sectionTitle")}</h2>
          <p className="text-muted-foreground max-w-2xl mx-auto">
            {t("sectionSubtitle")}
          </p>
        </div>

        <div className="grid grid-cols-1 md:grid-cols-2 lg:grid-cols-4 gap-8">
          {steps.map(({ key, icon: Icon }, i) => (
            <div key={key} className="relative text-center space-y-4">
              <div className="mx-auto h-16 w-16 rounded-full bg-primary/10 flex items-center justify-center relative z-10">
                <Icon className="h-8 w-8 text-primary" />
              </div>
              {i < steps.length - 1 && (
                <div className="hidden lg:block absolute top-8 left-[60%] w-[80%] h-px bg-border" />
              )}
              <div className="text-sm font-semibold text-primary">
                {String(i + 1).padStart(2, "0")}
              </div>
              <h3 className="font-semibold">{t(`${key}.title`)}</h3>
              <p className="text-sm text-muted-foreground">
                {t(`${key}.description`)}
              </p>
            </div>
          ))}
        </div>
      </div>
    </section>
  );
}
