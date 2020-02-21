using Microsoft.AspNetCore.Mvc.Rendering;

namespace IISAdministration.Helpers {

    public static class ViewHelper {

        /// <summary>
        /// Determines whether a given controller is selected. If it is selected then it will return the provided
        /// CSS string.
        /// </summary>
        /// <param name="htmlHelper">The extension class.</param>
        /// <param name="controller">The controller to check if selected.</param>
        /// <param name="action">The action to check if selected.</param>
        /// <param name="selectedClass">The CSS class which denotes a selected control.</param>
        /// <returns></returns>
        public static string IsSelected(this IHtmlHelper htmlHelper, string controller, string action, string selectedClass = "active") {
            string currentController = htmlHelper.ViewContext.RouteData.Values["controller"] as string;
            string currentAction = htmlHelper.ViewContext.RouteData.Values["action"] as string;
            return currentController.Equals(controller) && currentAction.Equals(action) ? selectedClass : string.Empty;
        }
    }
}
