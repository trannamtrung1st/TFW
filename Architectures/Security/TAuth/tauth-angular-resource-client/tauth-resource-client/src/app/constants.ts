import { cloneDeep } from "lodash";

import { RoutingData } from "@cross/routing/models/routing-data.model";

// Routing
const BASE_KEYWORD = 'base';
interface AppRouting {
    [BASE_KEYWORD]: string,
    home: string,
    profile: string,
    admin: string,
    accessDenied: string,
    notFound: string
}

export const ROUTING: AppRouting = {
    [BASE_KEYWORD]: '',
    home: '',
    profile: 'profile',
    admin: 'admin',
    accessDenied: 'access-denied',
    notFound: 'not-found'
};

const absRoutes: any = cloneDeep(ROUTING);
function makeAbsolute(entry: any, parent: string) {
    const prefix = parent + (entry[BASE_KEYWORD] ? `/${entry[BASE_KEYWORD]}` : entry[BASE_KEYWORD]);
    Object.keys(entry).forEach(key => {
        if (key === BASE_KEYWORD) {
            entry[key] = prefix;
        } else if (typeof entry[key] === 'string') {
            entry[key] = `${prefix}/${entry[key]}`;
        } else {
            makeAbsolute(entry[key], prefix);
        }
    });
}
makeAbsolute(absRoutes, '');

export const A_ROUTING: AppRouting = absRoutes;

export const ROUTING_DATA = {
    common: {
        accessDeniedPath: A_ROUTING.accessDenied,
    } as RoutingData
};