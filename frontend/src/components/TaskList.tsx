import React, { useState } from "react";
import { useTasks } from "@/services/websocket";
import { ProgressTask } from "@/services/openapi";
import { FontAwesomeIcon } from "@fortawesome/react-fontawesome";
import { faChevronDown } from "@fortawesome/free-solid-svg-icons";

export const TaskList = () => {
	const tasks: Array<ProgressTask> = useTasks();

    const [visible, setVisible] = useState<boolean>(true);
	const maxShownTasks = 5;
	const shownTasks = tasks.length > maxShownTasks ? tasks.slice(0, maxShownTasks) : tasks;

    return (
        <>
            {tasks.length > 0 && (
                <div className="fixed bottom-0 w-48 px-4 bg-[#2d2d30]">
                    <div className="text-xs flex align-middle space-x-2 items-center cursor-pointer" onClick={() => setVisible(!visible)}>
                        <hr className="flex-1 border-gray-500" />
                        <span className="text-gray-500">Tasks {tasks.length > maxShownTasks && `(${tasks.length})`}</span>
                        <hr className="flex-1 border-gray-500" />
                        <div className={`bg-gray-600 w-[1.5em] h-[1.5em] text-[0.75em] cursor-pointer flex items-center justify-center radius-100 arrow rotate ${visible && 'rotate-180'}`}>
                            <FontAwesomeIcon icon={faChevronDown}/>
                        </div>
                    </div>
                    <div className={`flex flex-col space-y-2 ${visible ? '' : 'hidden'}`}>
                        {shownTasks.map(i => (
                            <div key={i.id}>
                                <div className="flex text-xs font-semibold text-gray-400">
                                    <div className="flex-1">{i.name}</div>
                                    <div>{i.progress}%</div>
                                </div>
                                {i.description && <div title={i.description} className="text-gray-500 text-xs whitespace-nowrap overflow-ellipsis overflow-hidden">{i.description}</div>}
                                {i.indeterminate ? (
                                    <div className="relative h-[6px] mt-2 overflow-hidden">
                                        <div className="absolute h-[6px] bg-slate-400 animate-indeterminate" style={{ width: `${Math.max(i.progress, 20)}%`, transition: 'width .25s' }} role="progressbar"></div>
                                    </div>
                                ) : (
                                    <div className="h-[6px] mt-2 bg-slate-400" style={{ width: `${i.progress}%`, transition: 'width .25s' }} role="progressbar" aria-valuenow={i.progress} aria-valuemin={0} aria-valuemax={1}></div>
                                )}
                            </div>
                        ))}
                    </div>
                </div>
            )}
        </>
    )
}