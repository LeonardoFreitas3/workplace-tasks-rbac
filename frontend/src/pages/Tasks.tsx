import { useEffect, useState, useContext } from "react";
import api from "../api/axios";
import { AuthContext } from "../context/AuthContext";
import type { Task } from "../types/Task";
import { canDeleteTask, canEditTask } from "../utils/permissions";

interface UserDto {
  id: string;
  email: string;
}

export default function Tasks() {
  const { user, logout } = useContext(AuthContext);

  const [tasks, setTasks] = useState<Task[]>([]);
  const [users, setUsers] = useState<UserDto[]>([]);

  // Pagination & filter
  const [page, setPage] = useState(1);
  const [pageSize] = useState(5);
  const [totalCount, setTotalCount] = useState(0);
  const [statusFilter, setStatusFilter] = useState<string | "">("");

  const totalPages = Math.ceil(totalCount / pageSize);

  // Create
  const [title, setTitle] = useState("");
  const [description, setDescription] = useState("");
  const [createAssignedTo, setCreateAssignedTo] = useState("");

  // Edit
  const [editingTaskId, setEditingTaskId] = useState<string | null>(null);
  const [editTitle, setEditTitle] = useState("");
  const [editDescription, setEditDescription] = useState("");
  const [editStatus, setEditStatus] = useState("Pending");
  const [editAssignedTo, setEditAssignedTo] = useState("");

  // ===========================
  // Fetch Tasks
  // ===========================
  const fetchTasks = async () => {
    const params: any = { page, pageSize };

    if (statusFilter !== "") {
      params.status = statusFilter;
    }

    const response = await api.get("/Tasks", { params });

    setTasks(response.data.data);
    setTotalCount(response.data.totalCount);
  };

  // ===========================
  // Fetch Users (Admin/Manager only)
  // ===========================
  const fetchUsers = async () => {
    if (user?.role === "Admin" || user?.role === "Manager") {
      const response = await api.get("/Users");
      setUsers(response.data);
    }
  };

  useEffect(() => {
    fetchTasks();
  }, [page, statusFilter]);

  useEffect(() => {
    if (user?.role === "Admin" || user?.role === "Manager") {
      fetchUsers();
    }
  }, [user]);

  // ===========================
  // Create Task
  // ===========================
  const handleCreate = async (e: React.FormEvent) => {
    e.preventDefault();

    await api.post("/Tasks", {
      title,
      description,
      assignedToId:
        user?.role === "Admin" || user?.role === "Manager"
          ? createAssignedTo || null
          : null
    });

    setTitle("");
    setDescription("");
    setCreateAssignedTo("");
    fetchTasks();
  };

  // ===========================
  // Delete Task
  // ===========================
  const handleDelete = async (id: string) => {
    await api.delete(`/Tasks/${id}`);
    fetchTasks();
  };

  // ===========================
  // Start Edit
  // ===========================
  const startEdit = (task: Task) => {
    setEditingTaskId(task.id);
    setEditTitle(task.title);
    setEditDescription(task.description);
    setEditStatus(task.status);
    setEditAssignedTo(task.assignedToId ?? "");
  };

  // ===========================
  // Update Task
  // ===========================
  const handleUpdate = async (id: string) => {
    await api.put(`/Tasks/${id}`, {
      title: editTitle,
      description: editDescription,
      status: editStatus,
      assignedToId: editAssignedTo || null
    });

    setEditingTaskId(null);
    fetchTasks();
  };

  // ===========================
  // Status badge color
  // ===========================
  const statusColor = (status: string) => {
    switch (status) {
      case "Pending":
        return "bg-gray-200 text-gray-700";
      case "InProgress":
        return "bg-yellow-200 text-yellow-800";
      case "Done":
        return "bg-green-200 text-green-800";
      default:
        return "";
    }
  };

  return (
    <div className="min-h-screen bg-slate-50 p-6">
      <div className="max-w-6xl mx-auto">

        {/* HEADER */}
        <div className="flex justify-between items-center mb-8">
          <h2 className="text-2xl font-semibold text-slate-800">
            Tasks
          </h2>

          <div className="flex items-center gap-4">
            <span className="text-sm text-slate-500">
              Role: {user?.role}
            </span>

            <button
              onClick={logout}
              className="px-4 py-2 bg-slate-800 text-white rounded-lg hover:bg-slate-700 transition"
            >
              Logout
            </button>
          </div>
        </div>

        {/* CREATE FORM */}
        <div className="bg-white p-6 rounded-xl shadow mb-8">
          <h3 className="text-lg font-semibold mb-4 text-slate-700">
            Create Task
          </h3>

          <form onSubmit={handleCreate} className="grid md:grid-cols-4 gap-4">
            <input
              type="text"
              placeholder="Title"
              value={title}
              onChange={(e) => setTitle(e.target.value)}
              className="border border-slate-300 px-4 py-2 rounded-lg"
              required
            />

            <input
              type="text"
              placeholder="Description"
              value={description}
              onChange={(e) => setDescription(e.target.value)}
              className="border border-slate-300 px-4 py-2 rounded-lg"
              required
            />

            {(user?.role === "Admin" || user?.role === "Manager") && (
              <select
                value={createAssignedTo}
                onChange={(e) => setCreateAssignedTo(e.target.value)}
                className="border border-slate-300 px-4 py-2 rounded-lg"
              >
                <option value="">Unassigned</option>
                {users.map((u) => (
                  <option key={u.id} value={u.id}>
                    {u.email}
                  </option>
                ))}
              </select>
            )}

            <button
              type="submit"
              className="bg-slate-800 text-white rounded-lg hover:bg-slate-700 transition"
            >
              Create
            </button>
          </form>
        </div>

        {/* FILTER */}
        <div className="mb-4 flex justify-between items-center">
          <select
            value={statusFilter}
            onChange={(e) => {
              setPage(1);
              setStatusFilter(e.target.value);
            }}
            className="border border-slate-300 px-3 py-2 rounded-lg"
          >
            <option value="">All Status</option>
            <option value="Pending">Pending</option>
            <option value="InProgress">In Progress</option>
            <option value="Done">Done</option>
          </select>

          <span className="text-sm text-slate-500">
            Total: {totalCount}
          </span>
        </div>

        {/* TASK LIST */}
        <div className="bg-white rounded-xl shadow">

        {tasks.length === 0 ? (
            <div className="py-20 text-center">
            <p className="text-lg text-slate-400 font-medium">
                In this moment there are no tasks to show
            </p>
            <p className="text-sm text-slate-300 mt-2">
                Try creating a new task or adjusting the filters.
            </p>
            </div>
        ) : (
            <div className="divide-y">
            {tasks.map((task) => (
                <div key={task.id} className="p-5">

                {editingTaskId === task.id ? (
                    <div className="grid md:grid-cols-5 gap-3">

                    <input
                        value={editTitle}
                        onChange={(e) => setEditTitle(e.target.value)}
                        className="border px-3 py-2 rounded-lg"
                    />

                    <input
                        value={editDescription}
                        onChange={(e) => setEditDescription(e.target.value)}
                        className="border px-3 py-2 rounded-lg"
                    />

                    <select
                        value={editStatus}
                        onChange={(e) => setEditStatus(e.target.value)}
                        className="border px-3 py-2 rounded-lg"
                    >
                        <option value="Pending">Pending</option>
                        <option value="InProgress">In Progress</option>
                        <option value="Completed">Completed</option>
                    </select>

                    {(user?.role === "Admin" || user?.role === "Manager") && (
                        <select
                        value={editAssignedTo}
                        onChange={(e) => setEditAssignedTo(e.target.value)}
                        className="border px-3 py-2 rounded-lg"
                        >
                        <option value="">Unassigned</option>
                        {users.map((u) => (
                            <option key={u.id} value={u.id}>
                            {u.email}
                            </option>
                        ))}
                        </select>
                    )}

                    <div className="flex gap-2">
                        <button
                        onClick={() => handleUpdate(task.id)}
                        className="text-green-600 text-sm"
                        >
                        Save
                        </button>

                        <button
                        onClick={() => setEditingTaskId(null)}
                        className="text-gray-500 text-sm"
                        >
                        Cancel
                        </button>
                    </div>
                    </div>
                ) : (
                    <div className="flex justify-between items-center">

                    <div>
                        <p className="font-semibold text-slate-800">
                        {task.title}
                        </p>

                        <p className="text-sm text-slate-500">
                        {task.description}
                        </p>

                        {task.assignedToEmail && (
                        <p className="text-xs text-slate-400 mt-1">
                            Assigned to: {task.assignedToEmail}
                        </p>
                        )}

                        <span
                        className={`text-xs px-2 py-1 rounded-full mt-2 inline-block ${statusColor(task.status)}`}
                        >
                        {task.status}
                        </span>
                    </div>

                    <div className="flex gap-4 items-center">

                        {canEditTask(user, task) && (
                        <button
                            onClick={() => startEdit(task)}
                            className="text-blue-500 text-sm"
                        >
                            Edit
                        </button>
                        )}

                        {canDeleteTask(user, task) && (
                        <button
                            onClick={() => handleDelete(task.id)}
                            className="text-red-500 text-sm"
                        >
                            Delete
                        </button>
                        )}

                    </div>
                    </div>
                )}

                </div>
            ))}
            </div>
        )}

        </div>


        {/* PAGINATION */}
        <div className="flex justify-between items-center mt-6">

          <button
            disabled={page === 1}
            onClick={() => setPage(prev => prev - 1)}
            className="px-4 py-2 bg-slate-200 rounded disabled:opacity-50"
          >
            Previous
          </button>

          <span className="text-sm text-slate-600">
            Page {page} of {totalPages || 1}
          </span>

          <button
            disabled={page >= totalPages}
            onClick={() => setPage(prev => prev + 1)}
            className="px-4 py-2 bg-slate-200 rounded disabled:opacity-50"
          >
            Next
          </button>

        </div>

      </div>
    </div>
  );
}
