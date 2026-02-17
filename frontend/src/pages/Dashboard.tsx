import { useContext } from "react";
import { useNavigate } from "react-router-dom";
import { AuthContext } from "../context/AuthContext";

export default function Dashboard() {
  const { user, logout } = useContext(AuthContext);
  const navigate = useNavigate();

  return (
    <div className="min-h-screen bg-slate-50 flex items-center justify-center p-6">
      <div className="bg-white p-10 rounded-xl shadow-lg w-full max-w-md text-center space-y-6">
        <h1 className="text-2xl font-semibold text-slate-800">
          Admin Dashboard
        </h1>

        <p className="text-slate-500">
          Welcome, {user?.role}
        </p>

        <div className="space-y-4">
          <button
            onClick={() => navigate("/tasks")}
            className="w-full bg-slate-800 text-white py-2 rounded-lg hover:bg-slate-700 transition"
          >
            Manage Tasks
          </button>

          <button
            onClick={() => navigate("/users")}
            className="w-full border border-slate-800 text-slate-800 py-2 rounded-lg hover:bg-slate-100 transition"
          >
            Manage Users
          </button>
        </div>

        <button
          onClick={logout}
          className="text-sm text-red-500 hover:text-red-600"
        >
          Logout
        </button>
      </div>
    </div>
  );
}
