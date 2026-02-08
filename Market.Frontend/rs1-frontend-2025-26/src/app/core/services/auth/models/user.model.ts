export interface User {
  id: number;
  email: string;
  firstName: string;
  lastName: string;
  fullName: string;

  isAdmin: boolean;
  isManager: boolean;
  isEmployee: boolean;

  token: string;
  refreshToken: string;
  expiresAt: Date;
}


