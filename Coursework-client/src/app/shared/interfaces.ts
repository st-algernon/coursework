import { SearchBy } from "./services/api.service";

export interface SearchItemsQuery {
    query: string,
    searchBy: SearchBy
}

export interface CreateCommentCommand {
    text: string,
    authorId: string,
    itemId: string
}

export interface Media {
    name: string,
    mimeType: string,
    path: string,
    size: number,
}
