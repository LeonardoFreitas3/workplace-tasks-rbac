import type { User } from "../types/User";
import type { Task } from "../types/Task";

//Determines if a user can delete a task.
export const canDeleteTask = (user: User | null, task: Task) => {
  if (!user) return false;

  if (user.role === "Admin") return true;

  return task.createdById === user.userId;
};


// Determines if a user can edit a task.
export const canEditTask = (user: User | null, task: Task) => {
  if (!user) return false;

  if (user.role === "Admin") return true;
  if (user.role === "Manager") return true;

  return task.createdById === user.userId;
};
