import { Table, TableBody, TableHead, TableHeader, TableRow } from "@/components/ui/table";
import { FontAwesomeIcon } from "@fortawesome/react-fontawesome";
import { faChevronDown } from "@fortawesome/free-solid-svg-icons";
import React, { useState } from "react";
import { formatSizeUnits } from "@/components/manga/MangaHeader";
import { ChapterData } from "@/components/manga/volume/VolumeOverview";
import { ChapterColumn } from "@/components/manga/volume/ChapterColumn";

export function VolumeList({chapters, volume}: { chapters: ChapterData[], volume?: number | null }) {
	const [visible, setVisible] = useState<boolean>(false);

	const size = formatSizeUnits(chapters.reduce((v, c) => v + c.local?.sizeInBytes ?? 0, 0));

	const exists = chapters.filter(c => c.local).length ?? 0;

	return (
		<div className="bg-neutral-300/5 text-white border border-gray-300/20 rounded-lg m-4">
			<div className="flex items-center justify-between p-4">
				<h1 className="text-2xl font-bold">{volume ? `Volume ${volume}` : "No Volume"}</h1>
				<div className="flex items-center space-x-4">
					<span className="bg-[#313244] px-2 py-1 rounded">{exists}/{chapters.length}</span>
					<span>{size}</span>
					<div onClick={() => setVisible(!visible)}
						 className={`bg-gray-600 w-[2em] h-[2em] cursor-pointer flex items-center justify-center radius-100 arrow rotate ${visible && 'rotate-180'}`}>
						<FontAwesomeIcon icon={faChevronDown}/>
					</div>
				</div>
			</div>
			<div className="overflow-x-auto" style={{display: visible ? undefined : 'none'}}>
				<Table className="min-w-full">
					<TableHeader>
						<TableRow>
							<TableHead className="text-left">Chapter</TableHead>
							<TableHead className="text-left">Title</TableHead>
							<TableHead className="text-left">Released At</TableHead>
							<TableHead className="text-left">Status</TableHead>
							<TableHead className="text-left">Download</TableHead>
						</TableRow>
					</TableHeader>
					<TableBody>
						{chapters.map(chapter => (<ChapterColumn chapter={chapter} key={chapter.metadata.id}/>))}
					</TableBody>
				</Table>
			</div>
		</div>)
}

