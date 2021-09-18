import { UserModel } from "@cross/user/user.model";
import { AuthResult } from "./auth-result.model";

export class AuthContext {
    constructor(public isAuthenticated: boolean, public user?: UserModel,
        public authResult: AuthResult = new AuthResult()) {
    }
}