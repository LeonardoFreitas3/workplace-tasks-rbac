import React from "react";

interface PaginationProps {
  page: number;
  totalPages: number;
  onPageChange: (newPage: number) => void;
}

export default function Pagination({
  page,
  totalPages,
  onPageChange
}: PaginationProps) {
  if (totalPages <= 1) return null; // Hide if only one page

  return (
    <div className="flex justify-between items-center mt-6">

      <button
        disabled={page === 1}
        onClick={() => onPageChange(page - 1)}
        className="px-4 py-2 bg-slate-200 rounded disabled:opacity-50"
      >
        Previous
      </button>

      <span className="text-sm text-slate-600">
        Page {page} of {totalPages}
      </span>

      <button
        disabled={page >= totalPages}
        onClick={() => onPageChange(page + 1)}
        className="px-4 py-2 bg-slate-200 rounded disabled:opacity-50"
      >
        Next
      </button>

    </div>
  );
}
