/**     
 * @file ReturnCodes.cs
 *
 * @brief All the return codes used by FunctionJumper!
 *
 * All the return codes used by FunctionJumper! Getting the same ReturnCode from diffrent functions doesn't meen the same thing...
 *
 */

namespace FunctionJumper
{
    /** @enum FunctionJumper.ReturnCodes
     *  @brief All the return codes used by FunctionJumper!
     *
     * All the return codes used by FunctionJumper! Getting the same ReturnCode from diffrent functions doesn't meen the same thing...
     *
     */
    enum ReturnCodes
    {
        Success,                /**< Returned by all functions when successful. */
        NoSubOrUnSubWasMade,    /**< AddSub or RemoveSub had an exeption when tring to add the delegate to the event. */
        NeedAlteastOneSub,      /**< RemoveSub will not allow a function to have less than one sub. */
        NoSub,                  /**< RemoveSub has no subs to remove... did you even check the count? */
        NoMethodInfo,           /**< MethodInfo is null. A good practice is to check the result of the functions you call. */
        NoEvent,                /**< INTERNAL ERROR. IF YOU RECIVE THIS REPORT A BUG! */
        NoUnEvent,              /**< INTERNAL ERROR. IF YOU RECIVE THIS REPORT A BUG! */
        NoDelType,              /**< We couldn't make/find a delegte. If from AddSub or RemoveSub check your method info. If from OverrideFrom then report a bug! */
        CompileError,           /**< The code you gave us has errors when inserted into our template. You failed. Try again. (Unless I broke it this vrsion... :[)*/
        NoTypeForEvent,         /**< INTERNAL ERROR. IF YOU RECIVE THIS REPORT A BUG! */
        InitFailed              /**< We failed to invoke the init method... maybe its a bug in the code you passed... maybe we broke it this version... We'll never know... */
    }
}