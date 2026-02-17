import { useState, useContext } from "react";
import api from "../api/axios";
import { AuthContext } from "../context/AuthContext";
import { useNavigate } from "react-router-dom";
import { jwtDecode } from "jwt-decode";

export default function Login() {
  const { login } = useContext(AuthContext);
  const navigate = useNavigate();

  const [email, setEmail] = useState<string>("");
  const [password, setPassword] = useState<string>("");

  const handleSubmit = async (e: React.FormEvent) => {
    e.preventDefault();

    try {
      const response = await api.post("/Auth/login", {
        email,
        password
      });

      login(response.data.token);
      const role =
        response.data.token &&
        jwtDecode<any>(response.data.token)[
            "http://schemas.microsoft.com/ws/2008/06/identity/claims/role"
        ];

        if (role === "Admin") {
        navigate("/dashboard");
        } else {
        navigate("/tasks");
        }

    } catch {
      alert("Invalid credentials");
    }
  };

  return (
  <div className="min-h-screen flex items-center justify-center bg-slate-100">
    <div className="bg-white p-8 rounded-xl shadow-md w-full max-w-md">
      <h2 className="text-2xl font-semibold text-slate-800 mb-6 text-center">
        Sign in to your account
      </h2>

      <form onSubmit={handleSubmit} className="space-y-4">
        <input
          type="email"
          placeholder="Email"
          className="w-full px-4 py-2 border border-slate-300 rounded-lg focus:outline-none focus:ring-2 focus:ring-slate-400"
          onChange={(e) => setEmail(e.target.value)}
        />

        <input
          type="password"
          placeholder="Password"
          className="w-full px-4 py-2 border border-slate-300 rounded-lg focus:outline-none focus:ring-2 focus:ring-slate-400"
          onChange={(e) => setPassword(e.target.value)}
        />

        <button
          type="submit"
          className="w-full bg-slate-800 text-white py-2 rounded-lg hover:bg-slate-700 transition"
        >
          Login
        </button>
      </form>
    </div>
  </div>
);

}
