// Represents a task returned by the API
export interface Task {
  id: string;
  title: string;
  description: string;
  status: string;

  createdAt: string;
  updatedAt: string;

  createdById: string;
  assignedToId?: string | null;
  assignedToEmail?: string | null;
}
