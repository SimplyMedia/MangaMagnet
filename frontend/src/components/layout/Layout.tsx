import React from "react";
import Link from "next/link";
import { useTasks } from "@/services/websocket";

export const Layout = ({ children }: React.PropsWithChildren) => {
	const tasks = useTasks();
	const maxShownTasks = 5;
	const shownTasks = tasks.length > maxShownTasks ? tasks.slice(0, maxShownTasks) : tasks;

	return (
		<div className="flex min-h-screen bg-[#1e1e1e]">
			<div className="flex flex-col w-48 bg-[#2d2d30] p-4">
				<div className="flex-1 flex flex-col space-y-6">
					<Link href={"/"} className="text-white">Manga</Link>
					<div className="flex flex-col space-y-4">
						<a className="text-gray-300 hover:text-white" href="#">
							Add New
						</a>
						<a className="text-gray-300 hover:text-white" href="#">
							Library Import
						</a>
						<a className="text-gray-300 hover:text-white" href="#">
							Activity
						</a>
						<a className="text-gray-300 hover:text-white" href="#">
							Settings
						</a>
					</div>
				</div>

				{tasks.length > 0 && (
					<div>
						<div className="text-xs flex align-middle space-x-2 items-center">
							<hr className="flex-1 border-gray-500" />
							<span className="text-gray-500">Tasks {tasks.length > maxShownTasks && `(${tasks.length})`}</span>
							<hr className="flex-1 border-gray-500" />
						</div>
						<div className="flex flex-col space-y-2">
							{shownTasks.map(i => (
								<div key={i.id}>
									<div className="flex mb-1 text-xs font-semibold text-gray-400">
										<div className="flex-1">{i.name}</div>
										<div>{i.progress}%</div>
									</div>
									<div className="h-[6px] bg-slate-400" style={{ width: `${i.progress}%`, transition: 'width .25s' }} role="progressbar" aria-valuenow={i.progress} aria-valuemin={0} aria-valuemax={1}></div>
								</div>
							))}
						</div>
					</div>
				)}
			</div>
			<main className={"flex-1"}>
				{children}
			</main>
		</div>
	)
};
