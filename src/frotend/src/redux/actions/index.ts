import * as roomAction from "./roomAction";
import * as userAction from "./userAction";
import * as authAction from "./authAction";
import * as menuAction from "./menuAction";
import * as bookingAction from "./bookingAction";

export default {
    ...roomAction,
    ...userAction,
    ...authAction,
    ...menuAction,
    ...bookingAction
}