// models/problem-report.model.ts

// Za listu (ListProblemReportQueryDto)
export interface ProblemReportListItem {
  id: number;
  title: string;
  authorName: string;
  creationDate: Date;
  location?: string;
  categoryName: string;
  statusName: string;
  commentsCount: number;
  tasksCount: number;
  ratingsCount: number;
  shortDescription?: string;
}

// Za detalje (GetProblemReportByIdQueryDto)
export interface ProblemReportDetail {
  id: number;
  title: string;
  description: string;
  location?: string;
  creationDate: Date;
  userId: number;
  authorName: string;
  categoryId: number;
  categoryName: string;
  statusId: number;
  statusName: string;
  commentsCount: number;
  tasksCount: number;
  ratingsCount: number;
}

// Za kreiranje (CreateProblemReportCommand)
export interface CreateProblemReportCommand {
  title: string;
  userId: number;
  description: string;
  location?: string;
  categoryId: number;
  statusId: number;
}

// Za update (UpdateProblemReportCommand)
export interface UpdateProblemReportCommand {
  title?: string;
  description?: string;
  location?: string;
  categoryId?: number;
  statusId?: number;
}

// Filteri (ListProblemReportQuery)
export interface ProblemReportFilter {
  search?: string;
  userId?: number;
  categoryId?: number;
  statusId?: number;
  sortBy?: string;          // Dodano
  sortDirection?: string;  
  page?: number;
  pageSize?: number;
}

// Paginacija
export interface PageResult<T> {
  items: T[];
  totalCount: number;
  page: number;
  pageSize: number;
  totalPages: number;
  sortBy?: string;
  sortDirection?: string;
}