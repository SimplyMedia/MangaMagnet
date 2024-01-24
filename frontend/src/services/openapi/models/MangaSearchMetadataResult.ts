/* generated using openapi-typescript-codegen -- do no edit */
/* istanbul ignore file */
/* tslint:disable */
/* eslint-disable */
import type { MangaStatus } from './MangaStatus';
export type MangaSearchMetadataResult = {
    mangaDexId: string;
    anilistId?: string | null;
    myAnimeListId?: string | null;
    mangaUpdatesId?: string | null;
    title: string;
    description: string;
    coverUrl?: string | null;
    status: MangaStatus;
    year?: number | null;
};

