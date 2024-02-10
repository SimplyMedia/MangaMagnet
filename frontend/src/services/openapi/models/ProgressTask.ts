/* generated using openapi-typescript-codegen -- do no edit */
/* istanbul ignore file */
/* tslint:disable */
/* eslint-disable */
export type ProgressTask = {
    /**
     * Gets the unique identifier of the task.
     */
    id: number;
    /**
     * Gets or sets the name of the task.
     */
    name: string;
    /**
     * Gets or sets the description of the task.
     */
    description?: string | null;
    /**
     * Gets or sets the progress of the task.
     */
    progress: number;
    /**
     * `true` if the task is completed, `false` otherwise.
     */
    isCompleted: boolean;
    /**
     * `true` if the task is indeterminate, `false` otherwise.
     */
    indeterminate: boolean;
    total?: number | null;
    current: number;
};

