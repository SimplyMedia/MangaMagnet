import { ChapterMetadataResponse } from "@/services/openapi";
import React, { useState } from "react";
import { FontAwesomeIcon } from "@fortawesome/react-fontawesome";
import { faArrowDown } from "@fortawesome/free-solid-svg-icons";
import ChapterEntry from "@/components/manga/ChapterEntry";

export default function VolumeList({chapters}: { chapters: ChapterMetadataResponse[] }) {
	const [visible, setVisible] = useState<boolean>(false);

	const click = () => {
		setVisible(!visible)
	}

	const volumeNumber = chapters[0].volumeNumber;

	return (
		<div className={"border border-gray-600 m-4 cursor-pointer radius overflow-hidden"}
			 onClick={() => setVisible(!visible)}>
			<div className={`p-4 flex items-center bg-gray-800 border-gray-600 ${visible ? 'border-b' : ''}`}>
				<div className={"flex-1 font-bold"}>
					{volumeNumber === null ? 'No volume' : `Volume ${volumeNumber}`}
				</div>
				<div
					className={`bg-gray-600 w-[2em] h-[2em] flex items-center justify-center radius-100 arrow rotate ${visible && 'rotate-180'}`}>
					<FontAwesomeIcon icon={faArrowDown}/>
				</div>
			</div>
			<div style={{display: visible ? undefined : 'none'}}>
				{chapters.map(chapter => (
					<ChapterEntry chapter={chapter} key={chapter.id}/>
				))}
			</div>
		</div>
	)
}
