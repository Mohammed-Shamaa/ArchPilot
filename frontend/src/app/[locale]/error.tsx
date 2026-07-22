"use client";

import { Button } from "@/components/ui/button";
import Link from "next/link";
import { useLocale } from "next-intl";
import { AlertTriangle, Home } from "lucide-react";

export default function Error({
  error,
  reset,
}: {
  error: Error & { digest?: string };
  reset: () => void;
}) {
  const locale = useLocale();

  return (
    <div className="flex min-h-[60vh] items-center justify-center px-4">
      <div className="text-center space-y-6 max-w-md">
        <AlertTriangle className="h-16 w-16 mx-auto text-destructive" />
        <h2 className="text-2xl font-bold">Something went wrong</h2>
        <p className="text-muted-foreground">
          {error.message || "An unexpected error occurred. Please try again."}
        </p>
        <div className="flex items-center justify-center gap-4">
          <Button onClick={reset}>Try Again</Button>
          <Link href={`/${locale}`}>
            <Button variant="outline" className="gap-2">
              <Home className="h-4 w-4" />
              Home
            </Button>
          </Link>
        </div>
      </div>
    </div>
  );
}
