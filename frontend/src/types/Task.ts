export interface Task {
  id: string;
  title: string;
  description: string;
  status: string;
  createdById: string;
  assignedToId?: string;
  assignedToEmail?: string | null;
}
