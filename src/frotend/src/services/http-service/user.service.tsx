import axios, { AxiosResponse } from "axios";
import ResourseService from "../resourse.service";
import { Room } from "../../models/room";
import { User } from "../../models/user";
import { UserType } from "../../models/enums/usertype.enum";
import { Gender } from "../../models/enums/gender.enum";
import { UserFormProps } from "../../models/types";

export default class UserService {
    static api = '/auth';

    static async SignIn(user: UserFormProps): Promise<User | undefined> {
        return await axios.get(ResourseService.url + this.api + `/${user.login}&${user.password}`);
    }

    static async SignUp(user: User): Promise<User> {
        return await axios.post(ResourseService.url + this.api, user);
    }

    static async GetAllUsers(): Promise<User[]> {
        return await axios.get(ResourseService.url + this.api);
    }

    static async GetUserById(id: number): Promise<User> {
        return await axios.get(ResourseService.url + this.api + `${id}`);
    }
}