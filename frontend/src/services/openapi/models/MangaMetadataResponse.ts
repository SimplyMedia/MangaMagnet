/* generated using openapi-typescript-codegen -- do no edit */
/* istanbul ignore file */
/* tslint:disable */
/* eslint-disable */
import type { MangaStatus } from './MangaStatus';
export type MangaMetadataResponse = {
    id: string;
    displayTitle: string;
    aliases: Array<string>;
    status: MangaStatus;
    year?: number | null;
    author: string;
    artist: string;
    description: string;
    genres: Array<string>;
    tags: Array<string>;
    userScore: number;
    coverImageUrl?: string | null;
    anilistId?: number | null;
    mangaDexId: string;
    mangaUpdatesId?: string | null;
    myAnimeListId?: number | null;
    createdAt: string;
    updatedAt: string;
};

