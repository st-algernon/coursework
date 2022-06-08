import { SearchBy } from "./enums"
import { FieldTypes, UserRoles, UserStates } from "./types"

export interface LoginRequest {
    email: string
    password: string
}

export interface RegisterRequest {
    email: string
    name: string
    password: string
}

export interface RefreshTokenRequest {
    accessToken: string,
    refreshToken: string
}

export interface CollectionRequest {
    title: string,
    description: string,
    coverUrl?: string,
    topicId: string,
    ownerId: string,
    fieldVMs: Field[]
}

export interface CommentRequest {
    text: string,
    authorId: string,
    itemId: string
}

export interface SearchRequest {
    query: string,
    searchBy: SearchBy,
}

export interface AuthResponse {
    accessToken: string,
    refreshToken: string
}

export interface User {
    id: string,
    name: string,
    email: string,
    userRole: UserRoles,
    userState: UserStates
}

export interface Topic {
    id: string,
    name: string
}

export interface Field {
    name: string,
    fieldTypeId: string
}

export interface FieldType {
    id: string,
    name: string
}

export interface FieldWithTypeName
{
    id: string,
    name: string,
    typeName: FieldTypes
}

export interface FullField {
    id: string,
    name: string,
    typeName: FieldTypes,
    value?: string
}

export interface Media {
    name: string,
    mimeType: string,
    path: string,
    size: number,
}

export interface Tag {
    id: string,
    name: string
}

export interface ShortCollection {
    id: string,
    title: string,
    description: string,
    coverUrl?: string,
    topicName: string,
    tags: Tag[],
    ownerId: string
}

export interface Collection {
    id?: string,
    title: string,
    description: string,
    coverUrl?: string,
    topicId: string,
    ownerId: string,
    fieldVMs: Field[]
}

export interface UsersItem {
    countOfLikes: number,
    isLiked: boolean
}

export interface Item {
    id: string,
    title: string,
    coverUrl?: string,
    creationDate: string,
    collectionId: string,
    tagVMs: Tag[],
    fullFieldVMs: FullField[],
    usersItemVM: UsersItem,
    ownerId: string
}

export interface ShortItem {
    id?: string,
    title: string,
    coverUrl?: string,
    collectionId: string,
    tagNames: string[],
    fullFieldVMs: FullField[]
}

export interface Comment {
    id?: string,
    text: string,
    creationDate: string,
    authorVM: User,
    itemId: string
}