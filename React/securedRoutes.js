import { lazy } from "react";
const FAQForm = lazy(() => import("../components/faqs/FaqForm"));
import Faqs from "components/faqs/Faqs";
const ChooseProvider = lazy(() => import("../components/chooseprovider/ChooseProvider"));
const RateProvider = lazy(() => import("../components/rateprovider/RateProvider"));
const ProviderRatings = lazy(() => import("../components/rateprovider/ProviderRatings"));

const dashboardRoutes = [
  {
    path: "/dashboard",
    name: "Dashboards",
    icon: "uil-home-alt",
    header: "Navigation",
    children: [
      {
        path: "/dashboard/providerratings",
        name: "providerratings",
        element: ProviderRatings,
        roles: ["Provider"],
        exact: true,
        isAnonymous: false
      },
      {
        path: "/dashboard/rateprovider",
        name: "rateprovider",
        element: RateProvider,
        roles: ["Consumer"],
        exact: true,
        isAnonymous: false
      },
      {
        path: "/dashboard/chooseprovider",
        name: "chooseprovider",
        element: ChooseProvider,
        roles: ["Consumer"],
        exact: true,
        isAnonymous: false
      },
      {
        path: "/dashboard/faqadmin",
        name: "Faqs",
        element: FAQForm,
        roles: ["SYSAdmin", "Admin"],
        exact: true,
        isAnonymous: false
      },
      {
        path: "/dashboard/faqs",
        name: "Faqs Public",
        element: Faqs,
        roles: [],
        exact: true,
        isAnonymous: false
      }
    ]
  }
];

const allRoutes = [...dashboardRoutes, ...errorRoutes];

export default allRoutes;
