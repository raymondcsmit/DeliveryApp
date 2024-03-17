export interface ResponseResult<T> {
  success: boolean;
  message: string;
  statusCode: number;
  data: T;
}