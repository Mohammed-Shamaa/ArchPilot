import { HeroSection } from "@/components/landing/hero";
import { FeaturesSection } from "@/components/landing/features";
import { WorkflowSection } from "@/components/landing/workflow";
import { CTASection } from "@/components/landing/cta";

export default function Home() {
  return (
    <>
      <HeroSection />
      <FeaturesSection />
      <WorkflowSection />
      <CTASection />
    </>
  );
}
