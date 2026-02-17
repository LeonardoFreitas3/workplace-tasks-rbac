import type { User } from "../types/User";
import type { Task } from "../types/Task";

export const canDeleteTask = (user: User | null, task: Task) => {
  if (!user) return false;

  if (user.role === "Admin") return true;

  return task.createdById === user.userId;
};

export const canEditTask = (user: User | null, task: Task) => {
  if (!user) return false;

  if (user.role === "Admin") return true;
  if (user.role === "Manager") return true;

  return task.createdById === user.userId;
};
