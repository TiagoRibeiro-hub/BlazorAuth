using Microsoft.AspNetCore.Authorization;
using Microsoft.Extensions.DependencyInjection;
using System.Reflection;

namespace BlazorAuth.Shared;

public static class AuthorizationOptionsExtension
{
    public static IServiceCollection AddAuthorizationHandlerServices(this IServiceCollection services)
    {
        services.AddTransient<IAuthorizationHandler, AdminAuthorizationHandler>();
        services.AddTransient<IAuthorizationHandler, UserAuthorizationHandler>();
        services.AddTransient<IAuthorizationHandler, SendysAuthorizationHandler>();

        return services;
    }
    public static void AddSharedPolicies(this AuthorizationOptions options)
    {
        var policies = FindPolicies();
        options.TryToAddPolicies(policies);
    }

    private static IEnumerable<PolicyInformation> FindPolicies()
    {
        var policyProvider = typeof(Policies);
        return from method in policyProvider.GetMethods(BindingFlags.Public | BindingFlags.Static)
              // The method should configure the policy builder, not return a built policy.
               where method.ReturnType == typeof(void)
               let methodParameter = method.GetParameters()
               // The method has to accept the AuthorizationPolicyBuilder, and no other parameter.
               where methodParameter.Length == 1 && methodParameter[0].ParameterType == typeof(AuthorizationPolicyBuilder)
               select new PolicyInformation(method.Name, method);
    }

    private static void TryToAddPolicies(this AuthorizationOptions options, IEnumerable<PolicyInformation> policies)
    {
        foreach (var policy in policies)
        {
            try
            {
                options.AddPolicy(policy.Name, builder => { policy.Method.Invoke(null, new object[] { builder }); });
            }
            catch (Exception e)
            {
                options.AddPolicy(policy.Name, Policies.PolicyConfigurationFailedFallback);
            }
        }
    }

    private class PolicyInformation
    {
        internal string Name { get; }

        internal MethodInfo Method { get; }

        internal PolicyInformation(string name, MethodInfo method)
        {
            Name = name;
            Method = method;
        }
    }
}