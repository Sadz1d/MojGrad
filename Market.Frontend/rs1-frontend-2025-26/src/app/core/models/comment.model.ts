// JWT payload interfejs
export interface JwtPayloadDto {
  sub: string;        // user id (string)
  email: string;
  is_admin: string;   // "true" | "false"
  is_manager: string;
  is_employee: string;
  ver: string;
  iat: number;        // issued at (unix)
  exp: number;        // expires at (unix)
  aud: string;
  iss: string;
}

// models/comment.model.ts

export interface CommentListItem {
  id: number;
  text: string;
  userId: number;
  userName: string;
  userEmail?: string;
  userRole?: string;
  isAdmin?: boolean;
  isManager?: boolean;
  isEmployee?: boolean;
  reportId: number;
  reportTitle?: string;
  publicationDate: Date;
  edited?: boolean;
  lastModifiedDate?: Date;
}

export interface CommentDetail {
  id: number;
  text: string;
  userId: number;
  userName: string;
  userEmail?: string;
  userRole?: string;
  isAdmin?: boolean;
  isManager?: boolean;
  isEmployee?: boolean;
  reportId: number;
  reportTitle?: string;
  publicationDate: Date;
  edited: boolean;
  lastModifiedDate?: Date;
}

export interface CreateCommentCommand {
  reportId: number;
  userId: number;
  text: string;
}

export interface UpdateCommentCommand {
  id: number;
  text: string;
}

export interface CommentFilter {
  reportId?: number;
  userId?: number;
  search?: string;
  page?: number;
  pageSize?: number;
  sortBy?: string;
  sortDirection?: string;
}

export interface PageResult<T> {
  items: T[];
  totalCount: number;
  page: number;
  pageSize: number;
  totalPages: number;
}