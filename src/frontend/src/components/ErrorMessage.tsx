import { AlertCircle } from "lucide-react";

interface ErrorMessageProps {
  message: string;
  className?: string;
}

export default function ErrorMessage({ message, className = "" }: ErrorMessageProps) {
  return (
    <div className={`flex items-center space-x-2 p-4 bg-error-50 border border-error-200 rounded-lg ${className}`}>
      <AlertCircle className="h-5 w-5 text-error-600 flex-shrink-0" />
      <p className="text-sm text-error-700">{message}</p>
    </div>
  );
}