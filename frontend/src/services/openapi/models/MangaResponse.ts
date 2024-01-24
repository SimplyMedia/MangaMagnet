/* generated using openapi-typescript-codegen -- do no edit */
/* istanbul ignore file */
/* tslint:disable */
/* eslint-disable */
import type { ChapterMetadataResponse } from './ChapterMetadataResponse';
import type { ChapterResponse } from './ChapterResponse';
import type { MangaMetadataResponse } from './MangaMetadataResponse';
import type { VolumeResponse } from './VolumeResponse';
export type MangaResponse = {
    id: string;
    path: string;
    metadata: MangaMetadataResponse;
    chapterMetadata: Array<ChapterMetadataResponse>;
    chapters: Array<ChapterResponse>;
    volumes: Array<VolumeResponse>;
    createdAt: string;
    updatedAt: string;
};

