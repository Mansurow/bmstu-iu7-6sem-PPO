import axios, { AxiosResponse } from "axios";
import ResourseService from "../resourse.service";
import { Room } from "../../models/room";
import { User } from "../../models/user";
import { UserType } from "../../models/enums/usertype.enum";
import { Gender } from "../../models/enums/gender.enum";
import { UserFormProps } from "../../models/types";

export default class UserService {
    static api = '/auth';

    static MockUsers: User[] =[
        {
            id: 1,
            surname: "Ivan",
            name: "Ivan",
            firstname: "ivan",
            gender: Gender.Male,
            birthday: "17-03-2002",
            email: "user1",
            password: "password",
            role: UserType.User,
            phone: "12121"
        },
        {
            id: 2,
            surname: "Ivan",
            name: "Ivan",
            firstname: "ivan",
            gender: Gender.Male,
            birthday: "17-03-2002",
            email: "admin",
            password: "admin",
            role: UserType.Admin,
            phone: "12121"
        }
    ]

    static async SignIn(user: UserFormProps): Promise<User | undefined> {
        //return await axios.get(ResourseService.url + this.api + `/${user.login}&${user.password}`);
        return this.MockUsers.find(u => u.email === user.login && u.password === user.password);
    }

    static async SignUp(user: UserFormProps): Promise<User> {
        return await axios.get(ResourseService.url + this.api + `/${user.login}&${user.password}`);
        // return this.MockUsers[0];
    }
}