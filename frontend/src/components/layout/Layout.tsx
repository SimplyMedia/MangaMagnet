import React from "react";
import Link from "next/link";
import { TaskList } from "../TaskList";
import { MetadataService } from "@/services/openapi";

export const Layout = ({children}: React.PropsWithChildren) => {
	const refreshMetadata = () => {
		MetadataService.metadataUpdateAll()
			.catch(error => console.error('Error updating metadata:', error));
	};

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
						<a className="text-gray-300 hover:text-white" href="#">
							<button onClick={refreshMetadata}>Refresh Metadata</button>
						</a>

					</div>
				</div>

				<TaskList/>
			</div>
			<main className={"flex-1"}>
				{children}
			</main>
		</div>
	)
};
