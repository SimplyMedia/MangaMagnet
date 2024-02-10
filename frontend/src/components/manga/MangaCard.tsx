import Image from "next/image";
import Link from "next/link";
import { MangaResponse, MangaStatus } from "@/services/openapi";

export const statusColors: {
	[key in MangaStatus]: {
		title: string,
		style: { backgroundColor: string, color: string }
	}
} = {
	[MangaStatus.CANCELLED]: {title: "Cancelled", style: {backgroundColor: "#BF360C", color: "white"}},
	[MangaStatus.ON_GOING]: {title: "Continuing", style: {backgroundColor: "#66BB6A", color: "white"}},
	[MangaStatus.COMPLETED]: {title: "Completed", style: {backgroundColor: "#0277BD", color: "white"}},
	[MangaStatus.ON_HOLD]: {title: "On Hold", style: {backgroundColor: "#FFEE58", color: "black"}},
}

export const MangaCard = ({manga}: { manga: MangaResponse }) => {
	return (
		<Link className={"m-2"} href={`/manga/${manga.id}`}>
			<div className={"w-[200px] h-[300px] flex relative card-manga"}>
				<div className={"absolute w-[30px] h-[30px] right-0"}
					 style={{
						 border: `15px solid ${statusColors[manga.metadata.status].style.backgroundColor}`,
						 borderLeftColor: 'transparent',
						 borderBottomColor: 'transparent'
					 }}></div>

				<Image
					src={manga.metadata.coverImageUrl ?? '/placeholder-cover.webp'}
					alt={manga.metadata.displayTitle}
					className=""
					height="300"
					style={{
						objectFit: "cover",
					}}
					width="200"
				/>

				<div className={"absolute bottom-0 left-0 right-0 p-3 card-manga-title"}>
					{manga.metadata.displayTitle}
				</div>
			</div>
		</Link>
	)
}
