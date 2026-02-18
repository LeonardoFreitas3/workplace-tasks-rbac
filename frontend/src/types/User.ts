// Represents the authenticated user stored in AuthContext
export interface User {
  token: string; // JWT token used for authenticated API requests
  role: string;
  userId: string;
}
