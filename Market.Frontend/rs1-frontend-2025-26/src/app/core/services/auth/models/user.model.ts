export interface User {
  id: number;
  email: string;

  fullName: string;

  isAdmin: boolean;
  isManager: boolean;
  isEmployee: boolean;

  token: string;
  refreshToken: string;
  expiresAt: Date;
}


