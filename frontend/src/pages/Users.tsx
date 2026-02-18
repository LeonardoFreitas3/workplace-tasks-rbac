import { useEffect, useState, useContext } from "react";
import api from "../api/axios";
import { AuthContext } from "../context/AuthContext";

interface UserDto {
  id: string;
  email: string;
  role: string;
}

export default function Users() {
  const { user: currentUser } = useContext(AuthContext);

  const [users, setUsers] = useState<UserDto[]>([]);
  const [email, setEmail] = useState("");
  const [password, setPassword] = useState("");
  const [role, setRole] = useState("Member");

  // Fetch users
  const fetchUsers = async () => {
    const response = await api.get("/Users");
    setUsers(response.data);
  };

  useEffect(() => {
    fetchUsers();
  }, []);

  // Change role
  const changeRole = async (id: string, newRole: string) => {
    await api.put(`/Users/${id}/role`, {
      role: newRole
    });

    fetchUsers();
  };

  // Create user
  const createUser = async (e: React.FormEvent) => {
    e.preventDefault();

    await api.post("/Users", {
      email,
      password,
      role
    });

    setEmail("");
    setPassword("");
    setRole("Member");

    fetchUsers();
  };

  // Delete user
  const deleteUser = async (id: string) => {
    await api.delete(`/Users/${id}`);
    fetchUsers();
  };

  return (
    <div className="min-h-screen bg-slate-50 p-6">
      <div className="max-w-4xl mx-auto bg-white rounded-xl shadow p-8">

        {/* HEADER */}
        <h2 className="text-2xl font-semibold text-slate-800 mb-8">
          Manage Users
        </h2>

        {/* CREATE USER */}
        <div className="mb-10">
          <h3 className="text-lg font-semibold mb-4 text-slate-700">
            Create User
          </h3>

          <form onSubmit={createUser} className="grid md:grid-cols-4 gap-4">
            <input
              type="email"
              placeholder="Email"
              value={email}
              onChange={(e) => setEmail(e.target.value)}
              className="border border-slate-300 px-4 py-2 rounded-lg focus:outline-none focus:ring-2 focus:ring-slate-400"
              required
            />

            <input
              type="password"
              placeholder="Password"
              value={password}
              onChange={(e) => setPassword(e.target.value)}
              className="border border-slate-300 px-4 py-2 rounded-lg focus:outline-none focus:ring-2 focus:ring-slate-400"
              required
            />

            <select
              value={role}
              onChange={(e) => setRole(e.target.value)}
              className="border border-slate-300 px-4 py-2 rounded-lg"
            >
              <option value="Admin">Admin</option>
              <option value="Manager">Manager</option>
              <option value="Member">Member</option>
            </select>

            <button
              type="submit"
              className="bg-slate-800 text-white rounded-lg hover:bg-slate-700 transition"
            >
              Create
            </button>
          </form>
        </div>

        {/* Users list */}
        <div className="space-y-4">
          {users.map((user) => (
            <div
              key={user.id}
              className="flex flex-col md:flex-row md:justify-between md:items-center border-b pb-4 gap-4"
            >
              <div>
                <p className="font-medium text-slate-800">
                  {user.email}
                </p>
                <p className="text-sm text-slate-500">
                  {user.role}
                </p>
              </div>

              <div className="flex gap-3 items-center">

                {/* role select */}
                <select
                  value={user.role}
                  onChange={(e) =>
                    changeRole(user.id, e.target.value)
                  }
                  className="border border-slate-300 px-3 py-1 rounded-lg"
                >
                  <option value="Admin">Admin</option>
                  <option value="Manager">Manager</option>
                  <option value="Member">Member</option>
                </select>

                {/* delete button */}
                {currentUser?.userId !== user.id && (
                  <button
                    onClick={() => deleteUser(user.id)}
                    className="text-red-500 hover:text-red-600 text-sm"
                  >
                    Delete
                  </button>
                )}
              </div>
            </div>
          ))}
        </div>

      </div>
    </div>
  );
}
