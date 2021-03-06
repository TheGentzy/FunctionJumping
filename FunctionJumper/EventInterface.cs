﻿/**
 * @file EventInterface.cs
 *
 * @brief An interface to the events (which you cann't see without reflection!)
 *
 * This file contains the EventInterface. This is where you subscrie and unsuscribe to your events. Accsessed through @ref FunctionJumper.EventMaster's getEvent function. 
 *
 * @todo Omptimize code
 *
 */

using System;
using System.Reflection;

namespace FunctionJumper
{
    public class EventInterface
    {
        public MethodInfo eventSub { get; private set; }
        public MethodInfo eventUnSub { get; private set; }
        public string funcName { get; private set; }
        public Type funcType { get; private set; }
        public MethodInfo delegateMethod { get; private set; }
        public Type delegateType { get; private set; }
        public int subCount { get; private set; }

        internal int InitINTERNAL(MethodInfo sourceMethod, Type _funcType, MethodInfo _delegateMethod, string _funcName)
        {
            funcName = _funcName;
            funcType = _funcType;
            delegateMethod = _delegateMethod;

            //Invoke init function then add to overridden list
            try
            {
                funcType.GetMethod("InitEvent").Invoke(null, new object[] { sourceMethod });
                EventMaster.addOverriddenMethod(sourceMethod);
            }
            catch (Exception e)
            {
                Console.WriteLine(e.ToString());
                return (int)ReturnCodes.InitFailed;
            }

            eventSub = funcType.GetMethod("add_OnCalled");
            eventUnSub = funcType.GetMethod("remove_OnCalled");

            if (eventSub == null)
            {
                return (int)ReturnCodes.NoEvent;
            }
            if (eventUnSub == null)
            {
                return (int)ReturnCodes.NoUnEvent;
            }
            if (delegateMethod == null)
            {
                return (int)ReturnCodes.NoDelType;
            }

            //Declare our delegte type
            delegateType = delegateMethod.DeclaringType;
            subCount = 0;

            return (int)ReturnCodes.Success;
        }

        public int AddSub(MethodInfo newSub)
        {
            if (newSub == null)
            {
                return (int)ReturnCodes.NoMethodInfo;
            }

            Delegate subToAdd;
            try
            {
                //try making our delegate
                subToAdd = Delegate.CreateDelegate(delegateType, newSub);
            }
            catch (Exception e)
            {
                Console.WriteLine(e.ToString());
                return (int)ReturnCodes.NoDelType;
            }

            if (subToAdd == null)
            {
                return (int)ReturnCodes.NoDelType;
            }

            try
            {
                //Try to add the sub...
                eventSub.Invoke(null, new object[] { subToAdd });
            }
            catch (Exception e)
            {
                Console.WriteLine(e.ToString());
                return (int)ReturnCodes.NoSubOrUnSubWasMade;
            }

            //Increase sub count
            subCount++;
            return (int)ReturnCodes.Success;
        }

        public int RemoveSub(MethodInfo oldSub)
        {
            if (oldSub == null)
            {
                return (int)ReturnCodes.NoMethodInfo;
            }

            if (subCount == 0)
            {
                return (int)ReturnCodes.NoSub;
            }

            if (subCount == 1)
            {
                return (int)ReturnCodes.NeedAlteastOneSub;
            }

            Delegate subToRemove;

            try
            {
                //try making our delegate
                subToRemove = Delegate.CreateDelegate(delegateType, oldSub);
            }
            catch (Exception e)
            {
                Console.WriteLine(e.ToString());
                return (int)ReturnCodes.NoDelType;
            }

            if(subToRemove == null)
            {
                return (int)ReturnCodes.NoDelType;
            }

            try
            {
                //Try to add remove sub
                eventUnSub.Invoke(null, new object[] { subToRemove });
            }
            catch (Exception e)
            {
                Console.WriteLine(e.ToString());

                return (int)ReturnCodes.NoSubOrUnSubWasMade;
            }

            //Drop sub count
            subCount--;
            return (int)ReturnCodes.Success;
        }

    }
}