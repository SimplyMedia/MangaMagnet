/* generated using openapi-typescript-codegen -- do no edit */
/* istanbul ignore file */
/* tslint:disable */
/* eslint-disable */
import type { CancelablePromise } from '../core/CancelablePromise';
import { OpenAPI } from '../core/OpenAPI';
import { request as __request } from '../core/request';
export class TestService {
    /**
     * @param xVersion
     * @param v
     * @returns any Success
     * @throws ApiError
     */
    public static testProgressTask(
        xVersion?: string,
        v?: string,
    ): CancelablePromise<any> {
        return __request(OpenAPI, {
            method: 'POST',
            url: '/api/Test',
            headers: {
                'X-Version': xVersion,
            },
            query: {
                'v': v,
            },
        });
    }
    /**
     * @param id
     * @param chapterNumber
     * @param xVersion
     * @param v
     * @returns any Success
     * @throws ApiError
     */
    public static testTestDownload(
        id: string,
        chapterNumber: number,
        xVersion?: string,
        v?: string,
    ): CancelablePromise<any> {
        return __request(OpenAPI, {
            method: 'POST',
            url: '/api/Test/download/{id}/{chapterNumber}',
            path: {
                'id': id,
                'chapterNumber': chapterNumber,
            },
            headers: {
                'X-Version': xVersion,
            },
            query: {
                'v': v,
            },
        });
    }
}
