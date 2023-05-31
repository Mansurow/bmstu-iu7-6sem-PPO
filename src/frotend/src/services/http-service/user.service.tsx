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

    static async SignUp(user: UserFormProps): Promise<User> {
        return await axios.get(ResourseService.url + this.api + `/${user.login}&${user.password}`);
    }
}