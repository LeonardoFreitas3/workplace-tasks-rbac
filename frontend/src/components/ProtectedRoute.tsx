import { useContext } from "react";
import { Navigate } from "react-router-dom";
import type { ReactNode } from "react";
import { AuthContext } from "../context/AuthContext";


interface Props {
  children: ReactNode;
  requiredRole?: string; // Optional role restriction
}

export default function ProtectedRoute({ children, requiredRole }: Props) {
  const { user } = useContext(AuthContext);

  // Redirect to login if user is not authenticated
  if (!user) {
    return <Navigate to="/login" replace />;
  }

  // Redirect if user does not have the required role
  if (requiredRole && user.role !== requiredRole) {
    return <Navigate to="/tasks" replace />;
  }

  return children;
}
