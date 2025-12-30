// payload kako dolazi iz JWT-a
//export interface JwtPayloadDto {
  //sub: string;
  //email: string;
  //is_admin: string;
  //is_manager: string;
  //is_employee: string;
  //ver: string;
  //iat: number;
  //exp: number;
  //aud: string;
  //iss: string;
//}
// JWT payload – how it comes from MojGrad backend
export interface JwtPayloadDto {
  // standard JWT
  sub: string;        // user id
  email: string;
  iat: number;        // issued at (unix)
  exp: number;        // expires at (unix)
  aud: string;
  iss: string;

  // custom MojGrad claims
  is_admin: string;   // "true" | "false"
  is_manager: string;
  is_employee: string;
  ver: string;        // token version
}

