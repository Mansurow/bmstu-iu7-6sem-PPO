import { Gender } from "./enums/gender.enum";
import { UserType } from "./enums/usertype.enum";

export interface User 
{
    id: number,
    surname: string,
    name: string,
    firstname: string,
    birthday: string,
    gender: Gender
    email: string,
    phone: string,
    password: string,
    role: UserType
}
