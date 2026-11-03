export interface RegisterRequest {
  fullName: string;
  email: string;
  password: string;
}

export interface LoginRequest {
  email: string;
  password: string;
}

export interface AuthResponse {
  message: string;
  token?: string;
}

export interface ApiError {
  message?: string;
  title?: string;
  errors?: Record<string, string[]>;
}
