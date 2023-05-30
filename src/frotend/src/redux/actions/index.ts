import * as roomAction from "./roomAction";
import * as userAction from "./userAction";
import * as authAction from "./authAction";

export default {
    ...roomAction,
    ...userAction,
    ...authAction,
}