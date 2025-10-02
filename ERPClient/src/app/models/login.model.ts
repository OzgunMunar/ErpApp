export interface LoginModel {
    emailOrUserName: string,
    password: string
}

export const initialLoginModel: LoginModel = {
    emailOrUserName: "",
    password: ""
}